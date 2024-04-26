import { Outlet, Link } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import { Nav, Navbar } from "react-bootstrap";

const Layout = () => {
  return (
    <>
      <Navbar bg="success" expand="lg" data-bs-theme="dark">
        <Navbar.Brand style={{marginLeft:'10px', fontSize:'35px'}}>Todo Manager</Navbar.Brand>
        <Navbar.Brand style = {{marginLeft:'25%'}}></Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="nav-container">
          <div className="nav-container">
            <Nav.Link style = {{fontSize:'25px'}}as={Link} to="/">
              Kotisivu
            </Nav.Link>
            <Nav.Link style = {{fontSize:'25px'}}as={Link} to="/Tasks">
              Tehtävät
            </Nav.Link>
            <Nav.Link style = {{fontSize:'25px'}}as={Link} to="/Activities">
              Aktiviteetit
            </Nav.Link>
            </div>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
      <Outlet />
    </>
  );
};

export default Layout;
