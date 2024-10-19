import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstPurchasetypeComponent } from './pmr-mst-purchasetype.component';

describe('PmrMstPurchasetypeComponent', () => {
  let component: PmrMstPurchasetypeComponent;
  let fixture: ComponentFixture<PmrMstPurchasetypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstPurchasetypeComponent]
    });
    fixture = TestBed.createComponent(PmrMstPurchasetypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
