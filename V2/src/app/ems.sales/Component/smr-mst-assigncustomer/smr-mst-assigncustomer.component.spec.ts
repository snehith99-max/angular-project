import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstAssigncustomerComponent } from './smr-mst-assigncustomer.component';

describe('SmrMstAssigncustomerComponent', () => {
  let component: SmrMstAssigncustomerComponent;
  let fixture: ComponentFixture<SmrMstAssigncustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstAssigncustomerComponent]
    });
    fixture = TestBed.createComponent(SmrMstAssigncustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
