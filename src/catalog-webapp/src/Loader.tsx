import Backdrop from '@mui/material/Backdrop';
import CircularProgress  from '@mui/material/CircularProgress';

interface LoaderOptions {
    loading: boolean;
}

export default function Loader({loading}: LoaderOptions) {    
    return (
        <Backdrop
            sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
            open={loading}
        >
            <CircularProgress color="inherit" />
        </Backdrop>
    );
}
