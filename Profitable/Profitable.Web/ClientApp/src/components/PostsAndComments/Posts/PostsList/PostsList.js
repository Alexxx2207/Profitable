import { useState, useEffect, useCallback, useContext } from 'react';
import { useNavigate } from 'react-router-dom';

import { AuthContext } from '../../../../contexts/AuthContext';
import { MessageBoxContext } from '../../../../contexts/MessageBoxContext';

import { PostsListItem } from './PostsListItem/PostsListItem';

import { loadPostsPage } from '../../../../services/posts/postsService';

import { JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, POSTS_LIST_POSTS_IN_PAGE_COUNT } from '../../../../common/config';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import styles from './PostsList.module.css';
import { getUserDataByJWT } from '../../../../services/users/usersService';

export const PostsList = () => {
    

    const navigate = useNavigate();

    const [posts, setPosts] = useState([]);
    let page = 0;

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const { JWT, removeAuth } = useContext(AuthContext);
    
    const loadPosts = useCallback(() => {
        loadPostsPage(page, POSTS_LIST_POSTS_IN_PAGE_COUNT)
            .then(result => {
                if (result.length > 0) {
                    setPosts(posts => [...posts, ...result])
                }
            });
        page++;
    }, [page]);

    const handleScroll = useCallback((e) => {
        const scrollHeight = e.target.documentElement.scrollHeight;
        const currentHeight = e.target.documentElement.scrollTop + window.innerHeight;

        if (currentHeight >= scrollHeight - 1 || currentHeight === scrollHeight) {
            loadPosts();
        }
    }, [loadPosts]);

    const addPostClickHandler = () => {

        getUserDataByJWT(JWT)
            .then(result => {
                setPosts([]);
                navigate('/posts/create');
            })
            .catch(err => {
                if (JWT && err.message === JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE) {
                    setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, false);
                    removeAuth();
                    navigate('/login');
                } else {
                    setMessageBoxSettings('You should login before creating a post!', false);
                    navigate('/login');
                }
            });
    };

    useEffect(() => {
        loadPosts();
        window.addEventListener("scroll", handleScroll);

        return () => window.removeEventListener('scroll', handleScroll);
    }, [handleScroll, loadPosts]);

    return (
        <div className={styles.wrapper}>
            <div className={styles.createPostContainer}>
                <button className={styles.addPostButton} onClick={addPostClickHandler}>
                    <FontAwesomeIcon className={styles.iconPlus} icon={faCirclePlus} />
                    <div className={styles.addPostText}>
                        Add Post
                    </div>
                </button>
            </div>
            <h1 className={styles.discoverHeading}>Discover</h1>
            <div className={styles.postsList}>
                {posts.map(post => <PostsListItem key={post.guid} post={post} />)}
            </div>
        </div>
    );
}