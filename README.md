# MassTransit Competing Consumers

When two or more instances of the same MassTransit consumer service are running, we should ideally end up in a competing consumer scenario where messages are split between all running instances.

However in this case:

* if 2 instances are running, only 1 of the instance processes messages, while the other remains idle.
* if 4 instances are running, only 2 of the instance processes messages, while the other 2 remains idle.


---

* The consumer app has a single `DoSomethingConsumer`, that waits for 5 seconds and logs a message.
* The consumer is set to have a concurrency limit of 1, but the same behavior can be reproduced without a set concurrency limit.
* MassTransit v6.2.1

---

### Run this locally

```bash
cd ConsoleApp
dotnet user-secrets set "ASB" "--azure-servicebus-connection-string--"

# open 2 or more new terminal instances, 
# and run the following in each of them
cd WorkerApp
dotnet run

# once they're all running, go back to the first terminal session
# in "ConsoleApp", to "Send" messages to the queue
dotnet run
```
