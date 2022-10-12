import { useEffect } from "react";
import {
    Routes,
    Route,
    useLocation
} from "react-router-dom";

import { AuthContextProvider } from '../../contexts/AuthContext';
import { MessageBoxContextProvider } from '../../contexts/MessageBoxContext';
import { NavBar } from "../NavBar/NavBar";
import { Home } from '../Home/Home';
import { MarketsPage } from '../Markets/MarketsPage/MarketsPage';
import { PostDetails } from '../PostsAndComments/Posts/PostDetails/PostDetails';
import { Login } from "../Authentication/Login/Login";
import { Register } from "../Authentication/Register/Register";
import { ProfilePage } from "../User/ProfilePage/ProfilePage";
import { Logout } from "../Authentication/Logout";
import { NotFoundPage } from "../NotFoundPage/NotFoundPage";

import { CreatePost } from "../PostsAndComments/Posts/CreatePost/CreatePost";
import { EditPost } from "../PostsAndComments/Posts/EditPost/EditPost";

import { ProfileInfo } from "../User/ProfileInfo/ProfileInfo";

import { Calcualtors } from "../Calculators/Calculators";
import { NewsList } from "../News/NewsList";
import { NewsArticle } from "../News/NewsArticle/NewsArticle";
import { Calendars } from "../Calendars/Calendars";

// eslint-disable-next-line
import styles from './App.module.css';
import { AccountActivity } from "../User/AccountActivity/AccountActivity";
import { PostsExplorer } from "../PostsAndComments/Posts/PostsExplorer/PostsExplorer";
import { AccountStatistics } from "../User/AccountStatistics/AccountStatistics";
import { AddPositionsRecord } from "../User/AccountStatistics/AddPositionsRecord/AddPositionsRecord";


export function App() {

    const location = useLocation();

    useEffect(() => {
        window.scrollTo(0, 0);
    }, [location.pathname]);

    return (
        <AuthContextProvider location={location}>
           <MessageBoxContextProvider>
                <div className={styles.content}>
                    <div className={styles.navigation}>
                        <NavBar />
                    </div>
                    <div className={styles.page}>
                    <Routes>
                        <Route path="/" element={
                            <Home />
                        }>
                        </Route>

                        <Route path="/markets" element={
                            <MarketsPage />
                        }>
                        </Route>

                        <Route path="/posts" element={
                            <PostsExplorer />
                        }>
                        </Route>

                        <Route path="/posts/:postId" element={
                            <PostDetails />
                        }>
                        </Route>

                        <Route path="/posts/create" element={
                            <CreatePost />
                        }>
                        </Route>

                        <Route path="/login" element={
                            <Login />
                        }>
                        </Route>

                        <Route path="/register" element={
                            <Register />
                        }>
                        </Route>

                        <Route path="/logout" element={
                            <Logout />
                        }>
                        </Route>

                        <Route path="/users/:searchedProfileEmail" element={ <ProfilePage/> }>
                            <Route index element={ <ProfileInfo /> } />
                            <Route path="personal-info" element={ <ProfileInfo /> } />
                            <Route path="account-statistics" element={<AccountStatistics />} />
                            <Route path="account-activity" element={ <AccountActivity /> } />
                        </Route>

                        <Route path="/:searchedProfileEmail/positions-record/create" element={
                            <AddPositionsRecord />
                        }>
                        </Route>

                        <Route path="/posts/:postId/edit" element={
                            <EditPost />
                        }>
                        </Route>

                        <Route path="/calculators" element={
                            <Calcualtors />
                        }></Route>

                        <Route path="/news" element={
                            <NewsList />
                        }></Route>
                        
                        <Route path="/news/:newsTitle" element={
                            <NewsArticle />
                        }></Route>

                        <Route path="/calendars" element={
                            <Calendars />
                        }></Route>

                        <Route path="*" element={
                            <NotFoundPage />
                        }>
                        </Route>
                    </Routes>
                    </div>
                </div>
            </MessageBoxContextProvider>
        </AuthContextProvider>
    );
}
