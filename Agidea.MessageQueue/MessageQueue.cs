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
        private static readonly string MailerQueueName = ConfigurationManager.AppSettings["MailerQueueName"];

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

        public string GetQueueUrl(string queueName)
        {
            var request = new GetQueueUrlRequest
            {
                QueueName = MailerQueueName,
                QueueOwnerAWSAccountId = QueueOwnerAccountId
            };

            var getQueueUrlResponse = _sqsClient.GetQueueUrl(request);
            return getQueueUrlResponse.HttpStatusCode.Equals(HttpStatusCode.OK) ? getQueueUrlResponse.QueueUrl : string.Empty;
        }

        public bool SendMessages(List<Message> messages)
        {
            var queueUrl = GetQueueUrl(MailerQueueName);

            var entries = new List<SendMessageBatchRequestEntry>();

            foreach (var message in messages)
            {
                entries.Add(new SendMessageBatchRequestEntry
                {
                    Id = message.Id.ToString(),
                    MessageBody = message.Body,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        {
                            "MessageType",
                            new MessageAttributeValue
                            {
                                StringValue = MessageType.Email.ToString()
                            }
                        }
                    }
                }
                );
            }

            var sendMessageBatchRequest = new SendMessageBatchRequest
            {
                Entries = entries,
                QueueUrl = queueUrl
            };

            var sendMessageBatchResponse = _sqsClient.SendMessageBatch(sendMessageBatchRequest);

            // TODO: Handle failures.

            return sendMessageBatchResponse.HttpStatusCode.Equals(HttpStatusCode.OK);
        }

        public bool DeleteMessages(List<Message> messages)
        {
            var queueUrl = GetQueueUrl(MailerQueueName);
            var entries = new List<DeleteMessageBatchRequestEntry>();
            foreach (var message in messages)
            {
                entries.Add(new DeleteMessageBatchRequestEntry
                {
                    Id = message.Id.ToString(),
                    ReceiptHandle = message.ReceiptHandle,
                });
            }

            var deleteMessageBatchRequest = new DeleteMessageBatchRequest
            {
                Entries = entries,
                QueueUrl = queueUrl
            };

            var deleteMessageBatchResponse = _sqsClient.DeleteMessageBatch(deleteMessageBatchRequest);
            
            // TODO: Handle failures.
            
            return deleteMessageBatchResponse.HttpStatusCode.Equals(HttpStatusCode.OK);
        }

        public List<Message> ReceiveMessages()
        {
            var queueUrl = GetQueueUrl(MailerQueueName);
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
    }
}