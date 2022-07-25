import styles from './Post.module.css';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { WEB_API_BASE_URL } from '../../../../common/config';


export const Post = () => {

    const { postId } = useParams();
    const [ post, setPost ] = useState({
        title: '',
        content: '',
        author: '',
        postedOn: '',
        imageType: '',
        image: '',
        likes: [],
        comments: [],
    });

    const createImgURL = () => {
        const imageUrl = `data:image/${post.imageType};base64,${post.image}`;
        return imageUrl;
    }

    useEffect(() => {
        fetch(`${WEB_API_BASE_URL}/posts/${postId}`)
        .then(response => response.json())
        .then(result => setPost(result))
    }, [postId])

    return (
    <div className={styles.post}>
        <img src={createImgURL()} alt="" />
        <div className={styles.text}>
            <h1>{post.title}</h1>
            <p>{post.content}</p>
        </div>
        <div className={styles.information}>
            <div className={styles.author}>
                {post.author}
            </div>
            <div className={styles.postedOn}>
                {post.postedOn}
            </div>
        </div>
    </div>);
}