import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnSalesordereditComponent } from './smr-trn-salesorderedit.component';

describe('SmrTrnSalesordereditComponent', () => {
  let component: SmrTrnSalesordereditComponent;
  let fixture: ComponentFixture<SmrTrnSalesordereditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnSalesordereditComponent]
    });
    fixture = TestBed.createComponent(SmrTrnSalesordereditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
