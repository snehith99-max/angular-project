import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstPriceassigncustomerComponent } from './smr-mst-priceassigncustomer.component';

describe('SmrMstPriceassigncustomerComponent', () => {
  let component: SmrMstPriceassigncustomerComponent;
  let fixture: ComponentFixture<SmrMstPriceassigncustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstPriceassigncustomerComponent]
    });
    fixture = TestBed.createComponent(SmrMstPriceassigncustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
