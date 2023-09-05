import { URL } from 'node:url';
import { Context, HttpRequest, HttpResponse } from "@azure/functions";

const catalogApi = process.env.CATALOG_API;

export default async function (context: Context, req: HttpRequest): Promise<HttpResponse> {
    try {
        if (!catalogApi) {
            throw new Error('Missing environment variable CATALOG_API');
        }

        const relativeUrl = '/' + req.url.split('/api/')[1]; // e.g. /products or /products/1?param=value
        const url = new URL(relativeUrl, catalogApi);
        const response = await fetch(url);
        const products = await response.json();

        return {
            status: response.status,
            body: JSON.stringify(products),
        };
    } catch (error) {
        return {
            status: 500,
            body: JSON.stringify({
                error: error.message,
            }),
        };
    }
};
