import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnVisitcompleteComponent } from './crm-trn-visitcomplete.component';

describe('CrmTrnVisitmanagerComponent', () => {
  let component: CrmTrnVisitcompleteComponent;
  let fixture: ComponentFixture<CrmTrnVisitcompleteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnVisitcompleteComponent]
    });
    fixture = TestBed.createComponent(CrmTrnVisitcompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
