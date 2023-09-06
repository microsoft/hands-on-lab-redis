import { Link as RouterLink } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Link from '@mui/material/Link';

export default function NavBar() {
    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="static">
                <Toolbar>
                    <Link variant="h6" component={RouterLink} underline="none" color="inherit" to="/" sx={{ flexGrow: 1 }}>
                    Redis workshop demo
                    </Link>
                </Toolbar>
            </AppBar>
        </Box>
    );
}