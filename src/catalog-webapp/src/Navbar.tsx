import { useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Link from '@mui/material/Link';
import Button from '@mui/material/Button';
import BrowsingHistory from './BrowsingHistory';
import useLoginSimulation from './useLoginSimulation';

export default function NavBar() {
    const [userId] = useLoginSimulation();
    const [showHistory, setShowHistory] = useState<boolean>(false);

    const openHistoryModal = () => setShowHistory(true);
    const closeHistoryModal = () => setShowHistory(false);

    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="static">
                <Toolbar>
                    <Link variant="h6" component={RouterLink} underline="none" color="inherit" to="/" sx={{ flexGrow: 1 }}>
                    Redis workshop demo
                    </Link>
                    <Button color="inherit" onClick={openHistoryModal}>User {userId}</Button>
                    {showHistory && (
                        <BrowsingHistory onClose={closeHistoryModal} />
                    )}
                </Toolbar>
            </AppBar>
        </Box>
    );
}