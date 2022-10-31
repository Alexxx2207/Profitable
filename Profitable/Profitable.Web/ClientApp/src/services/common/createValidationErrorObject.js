import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from "../../common/config";

export const createClientErrorObject = (error, functionChecker) => {
    return {
        text: error.text,
        fulfilled: functionChecker(),
        type: CLIENT_ERROR_TYPE,
    };
};

export const createServerErrorObject = (errorMessage, display) => {
    return {
        text: errorMessage,
        display: display,
        type: SERVER_ERROR_TYPE,
    };
};
