# Redis workshop demo web app

## Run locally

Once you have an API running locally, you can run these commands to start the web app

```sh
# Install the project dependencies
npm install

# Build the web app
npm run swa:build

# Run the web app and use the local API
CATALOG_API=http://127.0.0.1:5076 HISTORY_API=http://127.0.0.1:7072/api npm run swa:start
```

