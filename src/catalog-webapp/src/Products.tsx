import Product from './Product';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';

interface ProductsOptions {
    list: Product[];
}

export default function Products({list}: ProductsOptions) {
    return (
        <Table sx={{ minWidth: 650 }} aria-label="List of products">
            <TableHead>
                <TableRow>
                    <TableCell>ID</TableCell>
                    <TableCell align="right">Title</TableCell>
                    <TableCell align="right">Description</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
            {list?.map((transcription) => (
                <TableRow key={transcription.id}>
                    <TableCell component="th" scope="row">{transcription.id}</TableCell>
                    <TableCell align="right">{transcription.title}</TableCell>
                    <TableCell align="right">{transcription.description}</TableCell>
                </TableRow>
            ))}
            </TableBody>
        </Table>
    );
}