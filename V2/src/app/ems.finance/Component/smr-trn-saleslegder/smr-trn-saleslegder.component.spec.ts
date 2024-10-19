import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnSaleslegderComponent } from './smr-trn-saleslegder.component';

describe('SmrTrnSaleslegderComponent', () => {
  let component: SmrTrnSaleslegderComponent;
  let fixture: ComponentFixture<SmrTrnSaleslegderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnSaleslegderComponent]
    });
    fixture = TestBed.createComponent(SmrTrnSaleslegderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
