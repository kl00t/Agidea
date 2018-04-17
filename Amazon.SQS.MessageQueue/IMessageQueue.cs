using System.Collections.Generic;

namespace Amazon.SQS.MessageQueue
{
    public interface IMessageQueue
    {
        string CreateQueue(string queueName);

        bool DeleteQueue(string queueUrl);

        string GetQueueUrl(string queueName);

        string SendMessage(string queueUrl, string messageBody);

        string SendMessages(string queueUrl);

        bool DeleteMessages(string queueUrl);

        bool DeleteMessage(string queueUrl, string receiptHandle);

        Dictionary<string, string> ReceiveMessages(string queueUrl);

        List<string> ListQueues();
    }
}
