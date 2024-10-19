import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnAnnualform22Component } from './hrm-trn-annualform22.component';

describe('HrmTrnAnnualform22Component', () => {
  let component: HrmTrnAnnualform22Component;
  let fixture: ComponentFixture<HrmTrnAnnualform22Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnAnnualform22Component]
    });
    fixture = TestBed.createComponent(HrmTrnAnnualform22Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
