import { useEffect, useCallback, useContext, useState } from "react";
import { useNavigate } from "react-router-dom";

import { PostsList } from "../PostsList/PostsList";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCirclePlus } from "@fortawesome/free-solid-svg-icons";
import {
    JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
    POSTS_LIST_POSTS_IN_PAGE_COUNT,
} from "../../../../common/config";
import { AuthContext } from "../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { loadPostsPage } from "../../../../services/posts/postsService";
import { getUserDataByJWT, getUserEmailFromJWT } from "../../../../services/users/usersService";

import { GoToTop } from "../../../Common/GoToTop/GoToTop";

import styles from "./PostsExplorer.module.css";

export const PostsExplorer = () => {
    const navigate = useNavigate();

    const [state, setState] = useState({
        posts: [],
        userEmail: "",
        page: 0,
        showShowMore: true,
    });

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const { JWT, removeAuth } = useContext(AuthContext);

    const loadPosts = useCallback((page, pageCount, userEmail) => {
        loadPostsPage(page, pageCount, userEmail).then((result) => {
            if (result.length > 0) {
                setState((oldState) => ({
                    ...oldState,
                    page: oldState.page + 1,
                    posts: [...oldState.posts, ...result],
                }));
            } else {
                setMessageBoxSettings("There are no more posts", false);
                setState((state) => ({
                    ...state,
                    showShowMore: false,
                }));
            }
        });
    }, []);

    const loadFirstPosts = useCallback((pageCount, userEmail) => {
        loadPostsPage(0, pageCount, userEmail).then((result) => {
            if (result.length > 0) {
                setState((oldState) => ({
                    ...oldState,
                    posts: [...result],
                }));
            } else {
                setState((state) => ({
                    ...state,
                    showShowMore: false,
                }));
            }
        });
    }, []);

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then((result) => {
                setState((oldState) => ({
                    ...oldState,
                    userEmail: result,
                }));
            })
            .catch((err) => err);
    }, [JWT]);

    useEffect(() => {
        loadFirstPosts(POSTS_LIST_POSTS_IN_PAGE_COUNT, state.userEmail);
    }, [loadFirstPosts, state.userEmail]);

    const handleShowMorePostsClick = useCallback(
        (e) => {
            e.preventDefault();
            setState((oldState) => ({
                ...oldState,
            }));
            loadPosts(state.page + 1, POSTS_LIST_POSTS_IN_PAGE_COUNT, state.userEmail);
        },
        [loadPosts, state.page]
    );

    const addPostClickHandler = () => {
        getUserDataByJWT(JWT)
            .then((result) => {
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

    return (
        <div className={styles.wrapper}>
            <div className={styles.createPostContainer}>
                <button className={styles.addPostButton} onClick={addPostClickHandler}>
                    <FontAwesomeIcon className={styles.iconPlus} icon={faCirclePlus} />
                    <div className={styles.addPostText}>Create Post</div>
                </button>
            </div>
            <div className={styles.explorerSection}>
                <h1 className={styles.discoverHeading}>Discover</h1>
                <h5 className={styles.discoverSubHeading}>
                    Check what other people have researched and want to say
                </h5>
                <div className={styles.postsListContainer}>
                    <PostsList posts={state.posts} />
                </div>
                {state.showShowMore ? (
                    <div className={styles.loadMoreContainer}>
                        <h4 className={styles.loadMoreButton} onClick={handleShowMorePostsClick}>
                            Show More Posts
                        </h4>
                    </div>
                ) : (
                    <></>
                )}
            </div>

            <GoToTop />
        </div>
    );
};
