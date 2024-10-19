import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstituteloginComponent } from './institutelogin.component';

describe('InstituteloginComponent', () => {
  let component: InstituteloginComponent;
  let fixture: ComponentFixture<InstituteloginComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InstituteloginComponent]
    });
    fixture = TestBed.createComponent(InstituteloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
