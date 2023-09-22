# Browsing history

## Overview

The Browsing history service (deployed as a Function App) provides an API to retrive the list of the latest products viewed by a user.

New browsing history events get added to the service as the user views new products in the UI. The catalog API will then share these events with this service using Redis Streams (identified by `PRODUCT_VIEWS_STREAM_NAME`).

Whenever a new event gets added to the stream, the service performs the following actions:
- Extract the data from the Redis Stream entry
- Serialize it
- Add the data to the history's Redis List of the user who viewed the product
- Ensure the Redis List is capped to the last 10 elements

```mermaid
sequenceDiagram
    actor U as User
    participant W as Web App
    participant C as Catalog API
    participant R as Redis
    participant H as History API
    U->>W: View a product
    W->>+C: Get product details
    C->>R: Add a product viewing entry <br>to Redis Streams
    H->>R: Get the product viewing event
    H->>H: Transform the event
    H->>R: Store the event in a capped Redis List
    C-->>-W: Product details
    W-->U: Product page
    U->>W: Get browsing history
    W->>+H: Get user browsing history
    H->>R: Fetch the browsing events' list
    R-->>H: List of serialized events
    H->>H: Tranform the event list
    H-->>-W: Browsing event list
    W-->>U: Browsing history
```

## Running locally

```sh
# Starting the function on the port 7072
func start -p 7072
```

## Publishing to Azure

```sh
func azure functionapp publish <Function App Name>
```