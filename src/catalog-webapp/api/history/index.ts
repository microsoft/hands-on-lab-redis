import { Context, HttpRequest, HttpResponse } from '@azure/functions';
import proxyApiCalls from '../proxyApiCalls';

export default async function (context: Context, req: HttpRequest): Promise<HttpResponse> {
    return proxyApiCalls(context, req, null, process.env.HISTORY_API);
};
