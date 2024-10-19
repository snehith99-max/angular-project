import { Injectable} from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class SharedService {
    private dataSubject = new BehaviorSubject<any>(null);
    private dataMenuOne = new BehaviorSubject<any>(null);
    private dataMenuTwo = new BehaviorSubject<any>(null);
    private dataMenuThree = new BehaviorSubject<any>(null);
    private dataMenuFour = new BehaviorSubject<any>(null);
    private dataMenuHeadPosition = new BehaviorSubject<any>(null);
    private dataMenuHead_Index = new BehaviorSubject<any>(null);
    private dataMenusecond_head_index = new BehaviorSubject<any>(null);
    private dataMenuPosition = new BehaviorSubject<any>(null);


    functionToCall!: () => void | any;
    function1ToCall!: () => void | any;
    menufunctionToCall!: () => void | any;
    sideNavCloseCall!: () =>void | any;
    menuOneTwo!: () => void | any;
    menuOne!: () => void | any;
    functionHeadToMenu!:()=>void | any

    setData(data: any){ this.dataSubject.next(data); }
    setMenuOne(data: any){ this.dataMenuOne.next(data); }
    setMenuTwo(data: any){ this.dataMenuTwo.next(data); }
    setMenuThree(data: any){ this.dataMenuThree.next(data); }
    setMenuFour(data: any){ this.dataMenuFour.next(data); }

    getData(){ return this.dataSubject.asObservable(); }
    getMenuOne(){ return this.dataMenuOne.asObservable(); }
    getMenuTwo(){ return this.dataMenuTwo.asObservable(); }
    getMenuThree(){ return this.dataMenuThree.asObservable(); }
    getMenuFour(){ return this.dataMenuFour.asObservable(); }

    setmenuHeadPosition(data: any){ this.dataMenuHeadPosition.next(data); }
    sethead_index(data: any){ this.dataMenuHead_Index.next(data); }
    setsecond_head_index(data: any){ this.dataMenusecond_head_index.next(data); }
    setmenuPosition(data: any){ this.dataMenuPosition.next(data); }
    setFunctionToCall(func: () => void){

        this.functionToCall = func;
        this.setFunction1ToCall(func);
    }

    setFunction1ToCall(func: () => void){
        this.function1ToCall = func;
    }

    setMenuToCall(func: () => void){
        this.menufunctionToCall = func;
    }

    setSideNavCloseCall(func: () => void){
        this.sideNavCloseCall = func;
    }

    selectMenuOneTwo(func: () => void){
        this.menuOneTwo = func;
    }

    selectMenuOne(func: () => void){
        this.menuOne = func;
    }
}