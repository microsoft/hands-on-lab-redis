import { URL } from 'node:url';
import { Context, HttpRequest, HttpResponse } from "@azure/functions";

export default async function (context: Context, req: HttpRequest, apiBaseUrl?: string, apiUrl?: string): Promise<HttpResponse> {
    try {
        console.log('CATALOG_API found', apiBaseUrl);

        if (!apiUrl && !apiBaseUrl) {
            throw new Error('Missing API endpoint and/or base url');
        }

        let url: URL;

        if (apiUrl) {
            url = new URL(apiUrl);
        }
        else {
            const relativeUrl = '/' + req.url.split('/api/')[1]; // e.g. /products or /products/1?param=value
            const baseUrl = apiBaseUrl.endsWith('/') ? apiBaseUrl : apiBaseUrl + '/';
            console.log('baseUrl ready :', baseUrl);
            console.log("relativeUrl ready:",relativeUrl);
            url = new URL(relativeUrl, baseUrl);
        }

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
