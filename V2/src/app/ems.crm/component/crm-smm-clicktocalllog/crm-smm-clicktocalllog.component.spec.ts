import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmClicktocalllogComponent } from './crm-smm-clicktocalllog.component';

describe('CrmSmmClicktocalllogComponent', () => {
  let component: CrmSmmClicktocalllogComponent;
  let fixture: ComponentFixture<CrmSmmClicktocalllogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmClicktocalllogComponent]
    });
    fixture = TestBed.createComponent(CrmSmmClicktocalllogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
