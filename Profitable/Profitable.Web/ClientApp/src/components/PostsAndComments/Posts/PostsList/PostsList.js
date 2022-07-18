import { useState, useEffect } from 'react';
import { Post } from '../Post/Post';

export const PostsList = () => {
    const [posts, setPosts] = useState([]);
    let page = 0;

    const loadPosts = () => {
        fetch(`${process.env.REACT_APP_API_BASE_URL}/api/posts/${page}`)
            .then(response => response.json())
            .then(result => {
                if (result.length > 0) {
                    setPosts(posts => [...posts, ...result])
                }
            });
        page++;
    }

    const handleScroll = (e) => {
        const scrollHeight = e.target.documentElement.scrollHeight;
        const currentHeight = e.target.documentElement.scrollTop + window.innerHeight;

        if (currentHeight >= scrollHeight - 1  || currentHeight == scrollHeight) {
            loadPosts();
            window.addEventListener('scroll', handleScroll);
        }
    }

    useEffect(() => {
        loadPosts();
        window.addEventListener("scroll", handleScroll);
    }, []);

    return (
        <div>
            <h1>Discover</h1>
            {posts.map(post => <Post key={post.guid} {...post} />)}
        </div>
    );
}