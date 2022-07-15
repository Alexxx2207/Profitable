import { NavLink } from 'react-router-dom';
import styles from './NavBar.module.css';

export const NavBar = () => {
    return (
        <nav className={styles.navbarContainer}>

            <NavLink to="/" className={styles.navbarItems}>
                <h1 className={styles.navbarLogo}>PROFITABLE</h1>
            </NavLink>
            <ul className={styles.navbarList}>
                <li>
                    <NavLink to="/" className={styles.navbarItems}>HOME</NavLink>
                </li>
                <li>
                    <NavLink to="/markets" className={styles.navbarItems}>MARKETS</NavLink>
                </li>
                <li>
                    <NavLink to="/posts" className={styles.navbarItems}>POSTS</NavLink>
                </li>
                <li>
                    <NavLink to="/about" className={styles.navbarItems}>ABOUT</NavLink>
                </li>
            </ul>
        </nav>);
}