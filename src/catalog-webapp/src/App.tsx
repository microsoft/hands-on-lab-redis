import {useState, useEffect} from 'react';
import Container from '@mui/material/Container';
import Backdrop from '@mui/material/Backdrop';
import CircularProgress  from '@mui/material/CircularProgress';
import NavBar from './Navbar';
import Product from './Product';
import Products from './Products';
import useFetch from './useFetch';

function App() {
  const [products, setProducts] = useState<Product[]>([]);
  const [initialProducts, loading] = useFetch<Product[]>('/api/products');

  useEffect(() => {
    if (Array.isArray(initialProducts)) {
      setProducts(initialProducts);
    }
  }, [initialProducts]);

  return (
    <>
      <NavBar />
      <main>
        <Container sx={{ paddingTop: 5 }}>
          <Products list={products} />
          <Backdrop
            sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
            open={loading}
          >
            <CircularProgress color="inherit" />
          </Backdrop>
        </Container>
      </main>
    </>
  );
}

export default App;
