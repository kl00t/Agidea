using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Agidea.Core.Interfaces;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Agidea.MessageQueue
{
    public class MessageQueue : IMessageQueue
    {
        private AmazonSQSClient _sqsClient;
        private readonly string ServiceUrl = ConfigurationManager.AppSettings["ServiceUrl"];
        private static string QueueOwnerAccountId = ConfigurationManager.AppSettings["QueueOwnerAccountId"];

        public MessageQueue()
        {
            if (_sqsClient == null)
            {
                CreateClient();
            }
        }

        public void CreateClient()
        {
            var sqsConfig = new AmazonSQSConfig
            {
                ServiceURL = ServiceUrl
            };

            _sqsClient = new AmazonSQSClient(sqsConfig);
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

        public string SendMessage(string queueUrl, string messageBody)
        {
            var sendMessageRequest = new SendMessageRequest(queueUrl, messageBody);
            var sendMessageResponse = _sqsClient.SendMessage(sendMessageRequest);
            return sendMessageResponse.HttpStatusCode.Equals(HttpStatusCode.OK) ? sendMessageResponse.MessageId : string.Empty;
        }

        public bool DeleteMessage(string queueUrl, string receiptHandle)
        {
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = receiptHandle,
            };

            var deleteMessageResponse = _sqsClient.DeleteMessage(deleteMessageRequest);
            return deleteMessageResponse.HttpStatusCode == HttpStatusCode.OK;
        }

        public bool DeleteMessages(string queueUrl)
        {
            var messages = ReceiveMessages(queueUrl);

            var entries = new List<DeleteMessageBatchRequestEntry>();
            foreach (var message in messages)
            {
                entries.Add(new DeleteMessageBatchRequestEntry
                {
                    Id = message.Key,
                    ReceiptHandle = message.Value
                });
            }

            var deleteMessageBatchRequest = new DeleteMessageBatchRequest
            {
                Entries = entries
            };

            var deleteMessageBatchResponse = _sqsClient.DeleteMessageBatch(deleteMessageBatchRequest);
            return deleteMessageBatchResponse.HttpStatusCode.Equals(HttpStatusCode.OK);
        }

        public Dictionary<string, string> ReceiveMessages(string queueUrl)
        {
            var messages = new Dictionary<string, string>();
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                AttributeNames = new List<string> { "All" },
                MaxNumberOfMessages = 5,
                QueueUrl = queueUrl,
                VisibilityTimeout = (int)TimeSpan.FromMinutes(10).TotalSeconds,
                WaitTimeSeconds = (int)TimeSpan.FromSeconds(5).TotalSeconds
            };

            var receiveMessageResponse = _sqsClient.ReceiveMessage(receiveMessageRequest);
            if (!receiveMessageResponse.HttpStatusCode.Equals(HttpStatusCode.OK))
            {
                return messages;
            }

            foreach (var message in receiveMessageResponse.Messages)
            {
                messages.Add(message.MessageId, message.ReceiptHandle);
            }

            return messages;
        }

        public string SendMessages(string queueUrl)
        {
            ////var sendMessageBatchRequest = new SendMessageBatchRequest();
            ////_sqsClient.SendMessageBatch(sendMessageBatchRequest);
            throw new NotImplementedException();
        }

        public List<string> ListQueues()
        {
            var listQueuesResponse = _sqsClient.ListQueues(new ListQueuesRequest());
            return listQueuesResponse.HttpStatusCode.Equals(HttpStatusCode.OK) ? listQueuesResponse.QueueUrls : new List<string>();
        }
    }
}