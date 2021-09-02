import { saveAs } from 'file-saver'
/**
 * 
 * @param {String} data [JSON字符串]
 * @param {String} filename [文件名]
 */
export function export_json(data, filename) {
    let file = new File([data], `${filename}.json`, {
        type: "text/plain;charset=utf-8"
    });
    file
    saveAs(file);
}