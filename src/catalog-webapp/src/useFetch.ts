import {useEffect, useState} from 'react';
import useLoginSimulation from './useLoginSimulation';

export default function useFetch<ResponseDataType>(url: string, method?: 'GET' | 'POST', body?: object | undefined) {
    const [controller] = useState(new AbortController());
    const [loading, setLoading] = useState(true);
    const [data, setData] = useState<ResponseDataType | undefined>();
    const [error, setError] = useState<any>();
    const [durationInMs, setDurationInMs] = useState<number | undefined>();
    const [userId] = useLoginSimulation();

    useEffect(() => {
        if (!userId) {
            return () => {};
        }

        (async () => {
            try {
                const start = Date.now();
                const options = {
                    signal: controller.signal,
                    method: method || 'GET',
                    body: body && !(body instanceof File) ? JSON.stringify(body) : body,
                    headers: {
                        'X-USER-ID': userId,
                    },
                };

                const response = await fetch(url, options);
                setDurationInMs(Date.now() - start);

                const responseData: ResponseDataType = await response.json();
                setData(responseData);
            } catch (requestError: any) {
                setError(requestError);
            } finally {
                setLoading(false);
            }
        })();
  
        return () => controller.abort();
    }, [url, method, body, userId, controller]);

    return [data, loading, error, durationInMs, controller];
}