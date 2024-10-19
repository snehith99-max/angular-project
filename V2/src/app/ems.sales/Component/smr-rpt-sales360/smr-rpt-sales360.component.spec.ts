import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptSales360Component } from './smr-rpt-sales360.component';

describe('SmrRptSales360Component', () => {
  let component: SmrRptSales360Component;
  let fixture: ComponentFixture<SmrRptSales360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptSales360Component]
    });
    fixture = TestBed.createComponent(SmrRptSales360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
