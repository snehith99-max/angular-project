import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnSalesAll360Component } from './smr-trn-sales-all360.component';

describe('SmrTrnSalesAll360Component', () => {
  let component: SmrTrnSalesAll360Component;
  let fixture: ComponentFixture<SmrTrnSalesAll360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnSalesAll360Component]
    });
    fixture = TestBed.createComponent(SmrTrnSalesAll360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
