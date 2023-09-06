import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';

interface CallDurationOptions {
    durationInMs: number;
}

export default function CallDuration({durationInMs}: CallDurationOptions) {    
    return (
        <Snackbar open={true}>
            <Alert severity="success" sx={{ width: '100%' }}>
            {`${durationInMs ?? 'N/A'} milliseconds`}
            </Alert>
        </Snackbar>
    );
}
