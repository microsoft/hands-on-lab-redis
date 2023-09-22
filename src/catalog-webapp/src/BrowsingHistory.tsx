import {useState} from 'react';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import CircularProgress  from '@mui/material/CircularProgress';
import Backdrop  from '@mui/material/Backdrop';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import useFetch from './useFetch';
import Loader from './Loader';

const maxEvents = 10;

interface BrowsingHistoryOptions {
    onClose: () => void;
}

interface BrowsingHistoryEvent {
    date: string;
    productId: string;
    productTitle: string;
}

export default function BrowsingHistory({onClose}: BrowsingHistoryOptions) {
    const [modalOpen, setModalOpen] = useState<boolean>(true);
    const [events, loading] = useFetch<BrowsingHistoryEvent[]>('/api/history');

    function onModalClose() {
        setModalOpen(false);
        onClose();
    }

    return (
        <Dialog open={modalOpen} fullWidth maxWidth="lg" onClose={onModalClose}>
            <DialogTitle>
                Browsing history
                <IconButton
                    aria-label="close"
                    onClick={onModalClose}
                    sx={{
                        position: 'absolute',
                        right: 8,
                        top: 8,
                        color: (theme) => theme.palette.grey[500],
                    }}
                    >
                    <CloseIcon />
                </IconButton>
            </DialogTitle>
            <DialogContent>
                <Table sx={{ minWidth: 650 }} aria-label="Browsing history">
                    <TableHead>
                        <TableRow>
                            <TableCell>Date</TableCell>
                            <TableCell align="right">Product ID</TableCell>
                            <TableCell align="right">Product Name</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                    {events?.slice(0, maxEvents - 1).map((event: BrowsingHistoryEvent) => (
                        <TableRow key={event.date}>
                            <TableCell component="th" scope="row">{new Date(event.date).toLocaleString()}</TableCell>
                            <TableCell align="right">{event.productId}</TableCell>
                            <TableCell align="right">{event.productTitle}</TableCell>
                        </TableRow>
                    ))}
                    </TableBody>
                </Table>
                <Loader loading={loading} />
            </DialogContent>
            <Backdrop sx={{ color: '#fff' }} open={loading}>
                <CircularProgress color="inherit" />
            </Backdrop>
        </Dialog>
    );
}
