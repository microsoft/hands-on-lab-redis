import {useEffect, useState} from 'react';

const userIdKey = 'hol-redis-user-id';

export default function useLoginSimulation() {
    const [userId, setUserId] = useState<string>('');

    useEffect(() => {
        (async () => {
            try {
                let id = window.sessionStorage.getItem(userIdKey);

                if (!id) {
                    id = window.crypto.randomUUID();
                    window.sessionStorage.setItem(userIdKey, id);
                }

                setUserId(id);
            } catch {
                // nothing
            }
        })();
    }, []);

    return [userId];
}