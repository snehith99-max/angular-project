import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnEmployee360Component } from './hrm-trn-employee360.component';

describe('HrmTrnEmployee360Component', () => {
  let component: HrmTrnEmployee360Component;
  let fixture: ComponentFixture<HrmTrnEmployee360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnEmployee360Component]
    });
    fixture = TestBed.createComponent(HrmTrnEmployee360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
