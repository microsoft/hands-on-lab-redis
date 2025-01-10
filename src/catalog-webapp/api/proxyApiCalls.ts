import { URL } from 'node:url';
import { Context, HttpRequest, HttpResponse } from "@azure/functions";

export default async function (context: Context, req: HttpRequest, apiBaseUrl?: string, apiUrl?: string): Promise<HttpResponse> {
    try {
        if (!apiUrl && !apiBaseUrl) {
            throw new Error('Missing API endpoint and/or base url');
        }

        let url: URL;

        if (apiUrl) {
            url = new URL(apiUrl);
        }
        else {
            const relativeUrl = './' + req.url.split('/api/')[1]; // e.g. /products or /products/1?param=value
            const baseUrl = apiBaseUrl.endsWith('/') ? apiBaseUrl : apiBaseUrl + '/';
            url = new URL(relativeUrl, baseUrl);
        }

        console.log(`Proxying ${req.url} to ${url}`);

        const response = await fetch(url, {
            headers: {
                'X-USER-ID': req.headers['x-user-id'],
            },
        });

        const data = await response.json();
        
        return {
            status: response.status,
            body: JSON.stringify(data),
        };
    } catch (error) {
        console.log('An error occurred', error);

        return {
            status: 500,
            body: JSON.stringify({
                error: error.message,
            }),
        };
    }
};
