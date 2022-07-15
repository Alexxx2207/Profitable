import { NavLink } from 'react-router-dom';


export const NavBar = () => {
    return (
        <nav className="">
            <ul>
                <li>
                    <NavLink to="/">Home</NavLink>
                </li>
                <li>
                    <NavLink to="/about">About</NavLink>
                </li>
                <li>
                    <NavLink to="/markets">Markets</NavLink>
                </li>
                <li>
                    <NavLink to="/posts">Posts</NavLink>
                </li>
            </ul>
        </nav>);
}