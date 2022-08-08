import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from '../../common/config';


export const createClientErrorObject = (error, funtionChecker) => {
    return {
        text: error.text,
        fulfilled: funtionChecker(),
        type: CLIENT_ERROR_TYPE
    };
}

export const createServerErrorObject = (errorMessage, display) => {
    console.log({
        text: errorMessage,
        display: display,
        type: SERVER_ERROR_TYPE
    });
    return {
        text: errorMessage,
        display: display,
        type: SERVER_ERROR_TYPE
    };
}