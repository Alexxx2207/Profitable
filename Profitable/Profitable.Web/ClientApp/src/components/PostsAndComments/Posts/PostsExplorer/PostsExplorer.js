import { useEffect, useCallback, useContext, useState } from "react";
import { useNavigate } from "react-router-dom";

import { PostsList } from "../PostsList/PostsList";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCirclePlus } from "@fortawesome/free-solid-svg-icons";
import { getUserDataByJWT, getUserEmailFromJWT } from "../../../../services/users/usersService";
import {
    JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
    POSTS_LIST_POSTS_IN_PAGE_COUNT,
} from "../../../../common/config";
import { AuthContext } from "../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { loadPostsPage } from "../../../../services/posts/postsService";

import { GoToTop } from "../../../GoToTop/GoToTop";

import styles from "./PostsExplorer.module.css";

export const PostsExplorer = () => {
    const navigate = useNavigate();

    const [posts, setPosts] = useState([]);
    const [userEmail, setUserEmail] = useState("");
    const [page, setPage] = useState(0);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const { JWT, removeAuth } = useContext(AuthContext);

    const loadPosts = useCallback((page, pageCount, userEmail) => {
        loadPostsPage(page, pageCount, userEmail).then((result) => {
            if (result.length > 0) {
                setPosts((posts) => [...posts, ...result]);
            }
        });
    }, []);

    const loadFirstPosts = useCallback((pageCount, userEmail) => {
        loadPostsPage(0, pageCount, userEmail).then((result) => {
            if (result.length > 0) {
                setPosts((posts) => [...result]);
            }
        });
    }, []);

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then((result) => {
                setUserEmail(result);
            })
            .catch((err) => err);
    }, [JWT]);

    useEffect(() => {
        loadFirstPosts(POSTS_LIST_POSTS_IN_PAGE_COUNT, userEmail);
    }, [loadFirstPosts, userEmail]);

    const handleScroll = useCallback(
        (e) => {
            const scrollHeight = e.target.documentElement.scrollHeight;
            const currentHeight = e.target.documentElement.scrollTop + window.innerHeight;

            if (currentHeight >= scrollHeight - 1 || currentHeight === scrollHeight) {
                setPage((state) => state + 1);
                loadPosts(page + 1, POSTS_LIST_POSTS_IN_PAGE_COUNT, userEmail);
            }
        },
        [loadPosts, setPage, page, userEmail]
    );

    const addPostClickHandler = () => {
        getUserDataByJWT(JWT)
            .then((result) => {
                setPosts([]);
                navigate("/posts/create");
            })
            .catch((err) => {
                if (JWT && err.message === JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE) {
                    setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, false);
                    removeAuth();
                    navigate("/login");
                } else {
                    setMessageBoxSettings("You should login before creating a post!", false);
                    navigate("/login");
                }
            });
    };

    useEffect(() => {
        window.addEventListener("scroll", handleScroll);

        return () => window.removeEventListener("scroll", handleScroll);
    }, [handleScroll]);

    return (
        <div className={styles.wrapper}>
            <div className={styles.createPostContainer}>
                <button className={styles.addPostButton} onClick={addPostClickHandler}>
                    <FontAwesomeIcon className={styles.iconPlus} icon={faCirclePlus} />
                    <div className={styles.addPostText}>Create Post</div>
                </button>
            </div>
            <h1 className={styles.discoverHeading}>Discover</h1>
            <div className={styles.postsListContainer}>
                <PostsList posts={posts} />
            </div>
            <GoToTop />
        </div>
    );
};
