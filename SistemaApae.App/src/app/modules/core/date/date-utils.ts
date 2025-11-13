export class DateUtils {
    static fromDbToDisplay(dataString: string): string {
        return new Date(dataString).toISOString().split('T')[0].split("-").reverse().join('/');
    }

    static fromDbToField(dataString: string): Date {
        const date = dataString.split("-")
        
        return new Date(date[0] as any, date[1] as any - 1, date[2] as any);
    }

    static fromFieldToDb(dataString: string): string {
        return  new Date(dataString).toISOString().slice(0, 10)
    }
}