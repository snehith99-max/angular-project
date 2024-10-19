import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstBranchwhatsappproductsummaryComponent } from './smr-mst-branchwhatsappproductsummary.component';

describe('SmrMstBranchwhatsappproductsummaryComponent', () => {
  let component: SmrMstBranchwhatsappproductsummaryComponent;
  let fixture: ComponentFixture<SmrMstBranchwhatsappproductsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstBranchwhatsappproductsummaryComponent]
    });
    fixture = TestBed.createComponent(SmrMstBranchwhatsappproductsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
