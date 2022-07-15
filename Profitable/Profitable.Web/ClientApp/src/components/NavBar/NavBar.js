import classNames from 'classnames';
import { NavLink } from 'react-router-dom';
import styles from './NavBar.module.css';

export const NavBar = () => {
    return (
        <nav className={styles.navbarContainer}>

            <div className={styles.logoWrapper}>
                <NavLink to="/" className={styles.navLink}>
                    <h1 className={styles.navbarLogo}>PROFITABLE</h1>
                </NavLink>
            </div>

            <ul className={styles.navbarList}>
                <li>
                    <NavLink to="/" className={classNames(styles.navbarListItems, styles.navLink)}>HOME</NavLink>
                </li>
                <li>
                    <NavLink to="/markets" className={classNames(styles.navbarListItems, styles.navLink)}>MARKETS</NavLink>
                </li>
                <li>
                    <NavLink to="/posts" className={classNames(styles.navbarListItems, styles.navLink)}>POSTS</NavLink>
                </li>
                <li>
                    <NavLink to="/about" className={classNames(styles.navbarListItems, styles.navLink)}>ABOUT</NavLink>
                </li>
            </ul>
        </nav>);
}