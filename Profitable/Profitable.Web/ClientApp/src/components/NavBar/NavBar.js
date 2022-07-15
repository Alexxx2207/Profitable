import classNames from 'classnames';
import { NavLink } from 'react-router-dom';
import styles from './NavBar.module.css';

export const NavBar = () => {
    return (
        <nav className={styles.navbarContainer}>

            <NavLink to="/" className={styles.navLink}>
                <h1 className={styles.navbarLogo}>PROFITABLE</h1>
            </NavLink>
            <ul className={styles.navbarList}>
                <li>
                    <NavLink to="/" activeClassName={styles.activeLink} className={classNames(styles.navbarListItems, styles.navLink)}>HOME</NavLink>
                </li>
                <li>
                    <NavLink to="/markets" activeClassName={styles.activeLink} className={classNames(styles.navbarListItems, styles.navLink)}>MARKETS</NavLink>
                </li>
                <li>
                    <NavLink to="/posts" activeClassName={styles.activeLink} className={classNames(styles.navbarListItems, styles.navLink)}>POSTS</NavLink>
                </li>
                <li>
                    <NavLink to="/about" activeClassName={styles.activeLink} className={classNames(styles.navbarListItems, styles.navLink)}>ABOUT</NavLink>
                </li>
            </ul>
        </nav>);
}