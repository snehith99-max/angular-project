import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnShopifycustomerComponent } from './smr-trn-shopifycustomer.component';

describe('SmrTrnShopifycustomerComponent', () => {
  let component: SmrTrnShopifycustomerComponent;
  let fixture: ComponentFixture<SmrTrnShopifycustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnShopifycustomerComponent]
    });
    fixture = TestBed.createComponent(SmrTrnShopifycustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
