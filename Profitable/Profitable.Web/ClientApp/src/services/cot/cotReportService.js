import { WEB_API_BASE_URL } from "../../common/config";
import { request } from "../../utils/fetch/request";

export const getReportedInstruments = async () => {
    var response = await request.get(`${WEB_API_BASE_URL}/cotreports/get/all-reports`);

    return response.json();
};

export const getReport = async (instrumentGuid, instrumentName, tuesdayDate) => {
    var response = await request.get(
        `${WEB_API_BASE_URL}/cotreports/get/report?InstrumentName=${encodeURIComponent(
            instrumentName
        )}&FromDate=${tuesdayDate}&InstrumentGuid=${instrumentGuid}`
    );

    return response.json();
};
