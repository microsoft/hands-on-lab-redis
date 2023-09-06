import { useParams } from 'react-router-dom';
import CssBaseline from '@mui/material/CssBaseline';
import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Product from './Product';
import useFetch from './useFetch';
import Loader from './Loader';
import CallDuration from './CallDuration';

export default function ProductView() {
    let { id } = useParams<'id'>();
    const [product, loading,, durationInMs] = useFetch<Product>(`/api/products/${id}`);

    return (
        <>
          {loading ? 'Loading...' : (
            <>
              <Typography
                component="h1"
                variant="h4"
                gutterBottom
              >
                {product.title}
              </Typography>
              <Grid container component="main" sx={{ height: '50vh' }}>
                <CssBaseline />
                <Grid
                  item
                  xs={12}
                  sm={6}
                  md={6}
                  sx={{
                    backgroundImage: `url(${product.image || 'azure-logo-color.svg'})`,
                    backgroundRepeat: 'no-repeat',
                    backgroundColor: (t) => t.palette.grey[50],
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                  }}
                />
                <Grid item xs={12} sm={6} md={6}>
                  <Box
                    sx={{
                      my: 8,
                      mx: 4,
                      display: 'flex',
                      flexDirection: 'column',
                    }}
                  >
                    <Typography paragraph={true}>
                      {product.description}
                    </Typography>
                    <Typography mt={2}>
                      <strong>Price:</strong> {product.price ? `${product.price} $` : 'N/A'}
                    </Typography>
                    <Typography mt={2}>
                      <strong>Quantity:</strong> {product.quantity || 'N/A'}
                    </Typography>
                  </Box>
                </Grid>
              </Grid>
            </>
          )}
          <Loader loading={loading} />
          {!loading && (
              <CallDuration durationInMs={durationInMs} />
          )}
        </>
    );
}
