import { URL } from 'node:url';
import { Context, HttpRequest, HttpResponse } from "@azure/functions";

export default async function (context: Context, req: HttpRequest, apiBaseUrl?: string): Promise<HttpResponse> {
    try {
        if (!apiBaseUrl) {
            throw new Error('Missing API base url');
        }

        const relativeUrl = './' + req.url.split('/api/')[1]; // e.g. /products or /products/1?param=value
        const baseUrl = apiBaseUrl.endsWith('/') ? apiBaseUrl : apiBaseUrl + '/';
        const url = new URL(relativeUrl, baseUrl);

        // Removing the connection header as per https://github.com/nodejs/undici/issues/1470
        const headers = {...req.headers};
        delete headers.connection;

        const response = await fetch(url, {
            headers,
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
