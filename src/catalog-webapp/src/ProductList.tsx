import {useState, useEffect} from 'react';
import { Link } from 'react-router-dom';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Product from './Product';
import useFetch from './useFetch';
import Loader from './Loader';
import CallDuration from './CallDuration';

export default function ProductList() {
    const [products, setProducts] = useState<Product[]>([]);
    const [initialProducts, loading,, durationInMs] = useFetch<Product[]>('/api/products');

    useEffect(() => {
        if (Array.isArray(initialProducts)) {
            setProducts(initialProducts);
        }
    }, [initialProducts]);
    
    return (
        <>
            <Table sx={{ minWidth: 650 }} aria-label="List of products">
                <TableHead>
                    <TableRow>
                        <TableCell>ID</TableCell>
                        <TableCell align="right">Title</TableCell>
                        <TableCell align="right">Description</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                {products?.map((product) => (
                    <TableRow key={product.id}>
                        <TableCell component="th" scope="row">
                            <Link to={`/${product.id}`}>{product.id}</Link>
                        </TableCell>
                        <TableCell align="right">{product.title}</TableCell>
                        <TableCell align="right">{product.description}</TableCell>
                    </TableRow>
                ))}
                </TableBody>
            </Table>
            <Loader loading={loading} />
            {!loading && (
                <CallDuration durationInMs={durationInMs} />
            )}
        </>
    );
}
