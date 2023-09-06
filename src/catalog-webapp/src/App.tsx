import type { RouteObject } from 'react-router-dom';
import { Outlet, useRoutes } from 'react-router-dom';
import Container from '@mui/material/Container';
import NavBar from './Navbar';
import ProductList from './ProductList';
import ProductView from './ProductView';

function Layout() {
  return (
    <>
      <NavBar />
      <main>
        <Container sx={{ paddingTop: 5 }}>
          <Outlet />
        </Container>
      </main>
    </>
  );
}

function App() {
  let routes: RouteObject[] = [
    {
      path: '/',
      element: <Layout />,
      children: [
        {
          index: true,
          element: <ProductList />,
        },
        {
          path: '/:id',
          element: <ProductView />,
        },
      ],
    },
  ];

  return useRoutes(routes);
}

export default App;
