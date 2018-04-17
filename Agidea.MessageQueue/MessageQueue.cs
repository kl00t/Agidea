using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Agidea.Core.Interfaces;
using Agidea.Core.Models;
using Amazon.SQS;
using Amazon.SQS.Model;
using AutoMapper;
using Message = Agidea.Core.Models.Message;

namespace Agidea.MessageQueue
{
    public class MessageQueue : IMessageQueue
    {
        private readonly IMapper _mapper;
        private readonly AmazonSQSClient _sqsClient;
        private static readonly string QueueOwnerAccountId = ConfigurationManager.AppSettings["QueueOwnerAccountId"];

        public MessageQueue(IMapper mapper)
        {
            if (_sqsClient == null)
            {
                var sqsConfig = new AmazonSQSConfig
                {
                    ServiceURL = ConfigurationManager.AppSettings["ServiceUrl"]
                };

                _sqsClient = new AmazonSQSClient(sqsConfig);
            }

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public string CreateQueue(string queueName)
        {
            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = queueName
            };

            var attrs = new Dictionary<string, string>
            {
                {
                    QueueAttributeName.VisibilityTimeout, "10"
                }
            };

            createQueueRequest.Attributes = attrs;
            var createQueueResponse = _sqsClient.CreateQueue(createQueueRequest);
            return createQueueResponse.HttpStatusCode.Equals(HttpStatusCode.OK) ? createQueueResponse.QueueUrl : string.Empty;
        }

        public bool DeleteQueue(string queueUrl)
        {
            var deleteQueueRequest = new DeleteQueueRequest
            {
                QueueUrl = queueUrl
            };

            var deleteQueueResponse = _sqsClient.DeleteQueue(deleteQueueRequest);
            return deleteQueueResponse.HttpStatusCode.Equals(HttpStatusCode.OK);
        }

        public string GetQueueUrl(string queueName)
        {
            var request = new GetQueueUrlRequest
            {
                QueueName = queueName,
                QueueOwnerAWSAccountId = QueueOwnerAccountId
            };

            var getQueueUrlResponse = _sqsClient.GetQueueUrl(request);
            return getQueueUrlResponse.HttpStatusCode.Equals(HttpStatusCode.OK) ? getQueueUrlResponse.QueueUrl : string.Empty;
        }

        public bool SendMessages(string queueUrl, List<Mail> mails)
        {
            var entries = new List<SendMessageBatchRequestEntry>();

            foreach (var mail in mails)
            {
                entries.Add(new SendMessageBatchRequestEntry
                {
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        {
                            "To", new MessageAttributeValue
                            {
                                StringListValues = mail.To
                            }
                        },
                        {
                            "CC", new MessageAttributeValue
                            {
                                StringListValues = mail.CC
                            }
                        },
                        {
                            "BCC", new MessageAttributeValue
                            {
                                StringListValues = mail.BCC
                            }
                        },
                        {
                            "Subject", new MessageAttributeValue
                            {
                                StringValue = mail.Subject
                            }
                        },
                        {
                            "Body", new MessageAttributeValue
                            {
                                StringValue = mail.Body
                            }
                        }
                    }
                });
            }

            var sendMessageBatchRequest = new SendMessageBatchRequest
            {
                QueueUrl = queueUrl,
                Entries = entries
            };

            var sendMessageBatchResponse = _sqsClient.SendMessageBatch(sendMessageBatchRequest);

            // TODO: Handle failures.

            return sendMessageBatchResponse.HttpStatusCode.Equals(HttpStatusCode.OK);
        }

        public bool DeleteMessages(string queueUrl, List<Message> messages)
        {
            var entries = new List<DeleteMessageBatchRequestEntry>();
            foreach (var message in messages)
            {
                entries.Add(new DeleteMessageBatchRequestEntry
                {
                    Id = message.Id,
                    ReceiptHandle = message.ReceiptHandle
                });
            }

            var deleteMessageBatchRequest = new DeleteMessageBatchRequest
            {
                Entries = entries
            };

            var deleteMessageBatchResponse = _sqsClient.DeleteMessageBatch(deleteMessageBatchRequest);
            
            // TODO: Handle failures.
            
            return deleteMessageBatchResponse.HttpStatusCode.Equals(HttpStatusCode.OK);
        }

        public List<Message> ReceiveMessages(string queueUrl)
        {
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                AttributeNames = new List<string> { "All" },
                MaxNumberOfMessages = 5,
                QueueUrl = queueUrl,
                VisibilityTimeout = (int)TimeSpan.FromMinutes(10).TotalSeconds,
                WaitTimeSeconds = (int)TimeSpan.FromSeconds(5).TotalSeconds
            };

            var receiveMessageResponse = _sqsClient.ReceiveMessage(receiveMessageRequest);
            return !receiveMessageResponse.HttpStatusCode.Equals(HttpStatusCode.OK) ? new List<Message>() : _mapper.Map<List<Message>>(receiveMessageResponse.Messages);
        }

        public List<string> ListQueues()
        {
            var listQueuesResponse = _sqsClient.ListQueues(new ListQueuesRequest());
            return listQueuesResponse.HttpStatusCode.Equals(HttpStatusCode.OK) ? listQueuesResponse.QueueUrls : new List<string>();
        }
    }
}